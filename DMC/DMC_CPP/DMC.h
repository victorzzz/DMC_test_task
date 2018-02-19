#pragma once
#include <ppl.h>
#include <concurrent_unordered_map.h>
#include <memory>
#include <typeindex>
#include "Utils/NonCopyable.h"
#include "Internal\Repository.h"
#include "IRepository.h"

namespace DMC_CPP
{
	class DMC : private Utils::NonCopyable
	{
	private:
		using RepositoriesMap = Concurrency::concurrent_unordered_map<std::type_index, std::shared_ptr<void>>;
		
		RepositoriesMap _repositories;

	public:
		DMC()
		{

		}

		DMC(DMC&& other) noexcept :
			_repositories(std::move(other._repositories))
		{

		}

		DMC& operator=(DMC&& other)
		{
			_repositories = std::move(other._repositories);
			return *this;
		}

		template<typename TEntity>
		std::shared_ptr<IRepository<TEntity>> Register()
		{
			auto insert_result = _repositories
					.insert(
						std::make_pair(
							std::type_index(typeid(TEntity)),
							std::make_shared<Internal::Repository<TEntity>>()));

			return std::static_pointer_cast<IRepository<TEntity>, void>(insert_result.first->second);
		}
	};
}