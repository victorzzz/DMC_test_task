#pragma once
#include <optional>
#include <vector>

namespace DMC_CPP
{
	template<typename TEntity>
	class IRepository
	{
	public:
		virtual long Update(const TEntity& entity, const std::optional<long>& id = std::optional<long>()) = 0;
		virtual long Update(TEntity&& entity, const std::optional<long>& id = std::optional<long>()) = 0;

		virtual TEntity& ReadById(const long& id) = 0;
		virtual const TEntity& ReadById(const long& id) const = 0;

		virtual std::vector<long> Ids() const = 0;
	};
}