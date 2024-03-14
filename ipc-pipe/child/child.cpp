// printout.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <thread>
#include <io.h>
#include <fcntl.h>
#include <vector>
#include <string>

using namespace std::chrono_literals;

int main(int argc, const char* argv[])
{
	// Set "stdin" to have binary mode:
	int result = _setmode(_fileno(stdin), _O_BINARY);
	if (result == -1) {
		std::cerr << "Cannot set mode\n";
		return EXIT_FAILURE;
	}
	else {
		std::cout << "'stdin' successfully changed to binary mode\n";
	}

	int len = std::stoi(argv[1]);

	std::vector<unsigned char> v(len, 0);
	std::cin.read(reinterpret_cast<char*>(&v[0]), len);

	for (int i = 0; i < 10; i++)
	{
		std::cerr << i << " seconds since epoch, value: 0x" << std::hex << int{ v[i] } << "\n";
		std::this_thread::sleep_for(1000ms);
	}
	return 0;
}
