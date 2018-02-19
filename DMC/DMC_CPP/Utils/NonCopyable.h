#pragma once

namespace DMC_CPP
{
	namespace Utils
	{
		struct NonCopyable 
		{
			NonCopyable() = default;

			NonCopyable(const NonCopyable&) = delete;
			const NonCopyable& operator=(const NonCopyable&) = delete;
		};
	}
}