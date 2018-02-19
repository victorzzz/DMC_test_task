#pragma once
#include "EntityBase.h"
#include <string>

namespace DMC_CPPTests
{
	namespace TestEntities
	{
		struct LaptopEntity : public EntityBase
		{
			LaptopEntity(const double& price, std::string&& model, long long ram, long long ssd, long long hdd)
				: EntityBase(price), Model(std::move(model)), RAM(ram), SSD(ssd), HDD(hdd)
			{
			}

			std::string Model;
			long long RAM;
			long long SSD;
			long long HDD;
		};
	}
}