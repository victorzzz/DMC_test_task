#pragma once
#include "IRepository.h"
#include <concurrent_unordered_map.h>
#include <Windows.h>
#include "Exceptions\EntityNotFoundException.h"
#include "Exceptions\UnexpectedErrorException.h"

namespace DMC_CPP
{
	namespace Internal
	{
		template <typename TEntity>
		class Repository : public IRepository<TEntity>
		{
		private:
			using Storage = Concurrency::concurrent_unordered_map<long, TEntity>;

			Storage _storage;

			volatile long _lastId = -1;

		public:
			long Update(const TEntity& entity, const std::optional<long>& id) override
			{
				return UpdateImpl(entity, id);
			}

			long Update(TEntity&& entity, const std::optional<long>& id) override
			{
				return UpdateImpl(std::move(entity), id);
			}

			TEntity& ReadById(const long& id) override
			{
				return Find(id)->second;
			}

			const TEntity& ReadById(const long& id) const override
			{
				return Find(id)->second;
			}

			std::vector<long> Ids() const override
			{
				std::vector<long> result;
				result.reserve(_lastId + 1);
				
				for (const auto& element : _storage)
				{
					result.push_back(element.first);
				}

				return result;
			}

		private:

			template<typename T>
			int UpdateImpl(T&& entity, const std::optional<long>& id)
			{
				if (id.has_value())
				{
					UpdateEntity(*id, std::forward<T>(entity));
					return *id;
				}
				else
				{
					return AddEntity(std::forward<T>(entity));
				}
			}

			typename Storage::iterator Find(long id)
			{
				auto find_result = _storage.find(id);
				if (find_result == _storage.end())
				{
					throw Exceptions::EntityNotFoundException(std::string("Entity not found. Id = ") + std::to_string(id));
				}

				return find_result;
			}

			typename Storage::const_iterator Find(long id) const
			{
				auto find_result = _storage.find(id);
				if (find_result == _storage.end())
				{
					throw Exceptions::EntityNotFoundException(std::string("Entity not found. Id = ") + std::to_string(id));
				}

				return find_result;
			}

			template<typename T>
			void UpdateEntity(long id, T&& entity)
			{
				Find(id)->second = std::forward<T>(entity);
			}

			template<typename T>
			long AddEntity(T&& entity)
			{
				auto id = ::InterlockedIncrement(&_lastId);
				auto insert_result = _storage.insert(std::make_pair(id, std::forward<T>(entity)));
				if (!insert_result.second)
				{
					throw Exceptions::UnexpectedErrorException("Unexpected internal error on an entity adding.");
				}

				return id;
			}
		};
	}
}