#pragma once

int** createMatrix(size_t const rows, size_t const columns);

void fillMatrixWithRandom(int** const matrix, size_t const rows, size_t const columns);

void printMatrix(int** const matrix, size_t const rows, size_t const columns);

int maxInMatrix(int** const matrix, size_t const rows, size_t const columns);

void freeMatrix(int** const matrix, size_t const rows);

