#pragma once
#include <exception>
#include <string>

namespace DMC_CPP
{
	namespace Exceptions
	{
		class EntityNotFoundException : public std::runtime_error
		{
		public:
			explicit EntityNotFoundException(const std::string& message) : std::runtime_error(message)
			{

			}
		};
	}
}