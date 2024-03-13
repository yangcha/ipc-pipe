// printout.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <thread>
#include <io.h>
#include <fcntl.h>

using namespace std::chrono_literals;

int main()
{
	// Set "stdin" to have binary mode:
	int result = _setmode(_fileno(stdin), _O_BINARY);
	if (result == -1) {
		std::cerr << "Cannot set mode\n";
	}
	else {
		std::cout << "'stdin' successfully changed to binary mode\n";
	}

	for (int i = 0; ; i++)
	{
		std::cerr << i << " seconds since epoch\n";
		std::this_thread::sleep_for(1000ms);
	}
}
