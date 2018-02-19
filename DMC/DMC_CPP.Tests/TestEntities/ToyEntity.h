#pragma once
#include "EntityBase.h"
#include <string>

namespace DMC_CPPTests
{
	namespace TestEntities
	{
		struct ToyEntity : public EntityBase
		{
			ToyEntity(const double& price, std::string&& name, int minAge)
				: EntityBase(price), Name(std::move(name)), MinAge(minAge)
			{
			}

			std::string Name;
			int MinAge;
		};
	}
}