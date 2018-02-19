#pragma once
namespace DMC_CPPTests
{
	namespace TestEntities
	{
		struct EntityBase
		{
			EntityBase(const double& price) : Price(price) {}

			// I know that it is bad idea to use double for money
			// but this is just an example of entity.
			// No calculations ( and related errors :) ) here
			double Price;
		};
	}
}