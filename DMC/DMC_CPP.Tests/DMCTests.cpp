#include "stdafx.h"

#include <ppltasks.h>
#include <vector>

#include "CppUnitTest.h"

#include "TestEntities\LaptopEntity.h"
#include "TestEntities\TVEntity.h"
#include "TestEntities\ToyEntity.h"

#include "DMC.h"

#include "memory"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace DMC_CPPTests
{		
	using namespace TestEntities;
	using namespace DMC_CPP;
	using namespace std;
	using namespace concurrency;

	static const TVEntity _testTV1 =
	{
		100.0,
		std::string("Model 1"),
		std::string("Resolution 1")
	},
		_testTV2 =
	{
		200.0,
		std::string("Model 2"),
		std::string("Resolution 2")
	},
	_testTV3 =
	{
		300.0,
		std::string("Model 3"),
		std::string("Resolution 3")
	};

	static const LaptopEntity _testLaptop1 =
	{
		100.0,
		std::string("Model 1"),
		8 * 1024ll,
		256 * 1024ll * 1024ll,
		512 * 1024ll * 1024ll
	},
	_testLaptop2 =
	{
		200.0,
		std::string("Model 2"),
		8 * 1024l,
		512 * 1024ll * 1024ll,
		512 * 1024ll * 1024ll
	},
	_testLaptop3 = 
	{
		3999.99,
		std::string("Model 3"),
		32l * 1024l,
		2048ll * 1024ll * 1024ll,
		4096ll * 1024ll * 1024ll
	};
	
	static const ToyEntity _testToy1 = 
	{
		15,
		"Toy 1",
		1
	},
	_testToy2 = 
	{
		25,
		"Toy 2",
		3
	},
	_testToy3 = 
	{
		135,
		"Toy 3",
		12
	};

	TEST_CLASS(CPP_DMCTests)
	{
	private:
		shared_ptr<DMC> _instance;

	public:
		
		TEST_METHOD_INITIALIZE(CPP_DMCTests_Initialize)
		{
			_instance = std::make_shared<DMC>();
		}

		TEST_METHOD(CPP_RegisterEntity_Add_Update_ReadById_NoException_CorrectIdAfterUpdate_CorrectRead)
		{
			// Arrange

			// Act
			auto laptopRepository = _instance->Register<LaptopEntity>();
			
			// add
			auto id1 = laptopRepository->Update(_testLaptop1);
			auto id2 = laptopRepository->Update(_testLaptop2);
			
			// update
			auto id2_after_update = laptopRepository->Update(_testLaptop3, id2);

			// read by id
			auto read_result1 = laptopRepository->ReadById(id1);
			auto read_result2 = laptopRepository->ReadById(id2);

			// Assert
			Assert::AreNotEqual(id1, id2);
			Assert::AreEqual(id2, id2_after_update);
			
			Assert::IsTrue(CompareLaptops(read_result1, _testLaptop1));
			Assert::IsTrue(CompareLaptops(read_result2, _testLaptop3));
		}

		TEST_METHOD(CPP_Multythread_RegisterEntities_Add_Update_ReadById_ReadAllIds_NoException_CorrectIdAfterUpdate_CorrectRead)
		{
			// Arrange / Act / Assert
			std::vector<task<void>> tasks;
			for (int i = 0; i < 16; ++i)
			{
				tasks.push_back(create_task([this]() -> void 
				{
					for (int j = 0; j < 100000; ++j)
					{
						auto toyRepository = _instance->Register<ToyEntity>();
						auto tvRepository = _instance->Register<TVEntity>();
						auto laptopRepository = _instance->Register<LaptopEntity>();

						/////////////////////////////
						// laptop
						////////////////////////////
						{
							// add
							auto id1 = laptopRepository->Update(_testLaptop1);
							auto id2 = laptopRepository->Update(_testLaptop2);

							// update
							auto id2_after_update = laptopRepository->Update(_testLaptop3, id2);

							// read by id
							auto read_result1 = laptopRepository->ReadById(id1);
							auto read_result2 = laptopRepository->ReadById(id2);

							// Assert
							Assert::AreNotEqual(id1, id2);
							Assert::AreEqual(id2, id2_after_update);

							Assert::IsTrue(CompareLaptops(read_result1, _testLaptop1));
							Assert::IsTrue(CompareLaptops(read_result2, _testLaptop3));
						}

						/////////////////////////////
						// tv
						////////////////////////////

						{
							// add
							auto idTv1 = tvRepository->Update(_testTV1);
							auto idTv2 = tvRepository->Update(_testTV2);

							// update
							auto idTv2_after_update = tvRepository->Update(_testTV3, idTv2);

							// read by id
							auto read_resultTv1 = tvRepository->ReadById(idTv1);
							auto read_resultTv2 = tvRepository->ReadById(idTv2);

							// Assert
							Assert::AreNotEqual(idTv1, idTv2);
							Assert::AreEqual(idTv2, idTv2_after_update);

							Assert::IsTrue(CompareTvs(read_resultTv1, _testTV1));
							Assert::IsTrue(CompareTvs(read_resultTv2, _testTV3));
						}
					}
				}));
			}

			when_all(begin(tasks), end(tasks)).wait();

			auto toyRepository = _instance->Register<ToyEntity>();
			auto tvRepository = _instance->Register<TVEntity>();
			auto laptopRepository = _instance->Register<LaptopEntity>();

			auto toyIds = toyRepository->Ids();
			auto tvIds = tvRepository->Ids();
			auto laptopIds = laptopRepository->Ids();

			Assert::AreEqual((size_t)0, toyIds.size());
			Assert::AreEqual((size_t)16 * 100000 * 2, tvIds.size());
			Assert::AreEqual((size_t)16 * 100000 * 2, laptopIds.size());
		}

	private:
		bool CompareLaptops(const LaptopEntity& laptop1, const LaptopEntity& laptop2)
		{
			return
				(laptop1.Price == laptop2.Price)
				&&
				(laptop1.Model == laptop2.Model)
				&&
				(laptop1.RAM == laptop2.RAM)
				&&
				(laptop1.SSD == laptop2.SSD)
				&&
				(laptop1.HDD == laptop2.HDD);
		}

		bool CompareTvs(const TVEntity& tv1, const TVEntity& tv2)
		{
			return
				(tv1.Price == tv2.Price)
				&&
				(tv1.Model == tv2.Model)
				&&
				(tv1.Resolution == tv2.Resolution);
		}
	};
}