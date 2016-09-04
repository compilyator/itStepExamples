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
	int sum = sumMatrix(matrix, rows, columns);
	std::cout << "Sum items of array = " << sum << std::endl;
	_getch();
	freeMatrix(matrix, rows);
}
