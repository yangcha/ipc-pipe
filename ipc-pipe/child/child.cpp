// printout.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <thread>

using namespace std::chrono_literals;

int main()
{
	for (int i = 0; ; i++)
	{
		std::cerr << i << " seconds since epoch\n";
		std::this_thread::sleep_for(1000ms);
	}
}
