#include <iostream>
#include <conio.h>
#include "Matrix.h"

void main()
{
	size_t rows, columns;
	std::cout << "Enter rows count: ";
	std::cin >> rows;
	std::cout << "Enter columns count: ";
	std::cin >> columns;
	int** matrix = createMatrix(rows, columns);
	fillMatrixWithRandom(matrix, rows, columns);
	std::cout << "Generated matrix " << rows << "x" << columns << ":" << std::endl;
	printMatrix(matrix, rows, columns);
	int max = maxInMatrix(matrix, rows, columns);
	std::cout << "Max item of array = " << max << std::endl;
	_getch();
	freeMatrix(matrix, rows);
}
