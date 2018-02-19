#pragma once
#include "EntityBase.h"
#include <string>
#include <memory>

namespace DMC_CPPTests
{
	namespace TestEntities
	{
		struct TVEntity : public EntityBase
		{
			TVEntity(const double& price, std::string&& model, std::string&& resolution)
				: EntityBase(price), Model(std::move(model)), Resolution(std::move(resolution))
			{

			}

			std::string Model;
			std::string Resolution;
		};
	}
}