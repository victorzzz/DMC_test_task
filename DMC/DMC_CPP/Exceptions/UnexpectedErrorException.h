#pragma once
#include <exception>
#include <string>

namespace DMC_CPP
{
	namespace Exceptions
	{
		class UnexpectedErrorException : public std::runtime_error
		{
		public:
			explicit UnexpectedErrorException(const std::string& message) : std::runtime_error(message)
			{

			}
		};
	}
}