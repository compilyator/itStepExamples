#include <iostream>
#include <time.h>
#include <iomanip>


int** createMatrix(size_t const rows, size_t const columns)
{
	int** matrix = new int*[rows];
	for (size_t i = 0; i < rows; i++)
	{
		matrix[i] = new int[columns];
	}
	return matrix;
}

void fillMatrixWithRandom(int** const matrix, size_t const rows, size_t const columns)
{
	srand(time(NULL));
	for (size_t i = 0; i < rows; i++)
	{
		for (size_t j = 0; j < columns; j++)
		{
			matrix[i][j] = rand() % 100;
		}
	}
}

void printMatrix(int** const matrix, size_t const rows, size_t const columns)
{
	for (size_t i = 0; i < rows; i++)
	{
		for (size_t j = 0; j < columns; j++)
		{
			std::cout << std::setw(4) << matrix[i][j];
		}
		std::cout << std::endl;
	}
}

int sumMatrix(int** const matrix, size_t const rows, size_t const columns)
{
	int sum = 0;
	for (size_t i = 0; i < rows; i++)
	{
		for (size_t j = 0; j < columns; j++)
		{
			sum += matrix[i][j];
		}
	}
	return sum;
}


void freeMatrix(int** const matrix, size_t const rows)
{
	for (size_t i = 0; i < rows; i++)
	{
		delete[] matrix[i];
	}
	delete[] matrix;
}