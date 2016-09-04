#include <iostream>
#include <iomanip>
#include <conio.h>
struct date
{
	int year;
	int month;
	int day;
};

struct student
{
	char firstName[10];
	char lastName[10];
	date dateOfBirth;
	int sex;
};

void EnterStudents(int const studentsCount, student* const students);

void PrintStudents(int const studentsCount, student* const students);

void main()
{
	int studentsCount;
	std::cout << "Enter students count: ";
	std::cin >>  studentsCount;
	student* students = new student[studentsCount];
	EnterStudents(studentsCount, students);
	PrintStudents(studentsCount, students);
	_getch();
	delete[] students;
}

void EnterStudents(int const studentsCount, student* const students)
{
	for (size_t i = 0; i < studentsCount; i++)
	{
		std::cout << "Enter student #" << i + 1 << " first name: ";
		std::cin >> students[i].firstName;
		std::cout << "Enter student #" << i + 1 << " last name: ";
		std::cin >> students[i].lastName;
		std::cout << "Enter student #" << i + 1 << " date of birth: ";
		std::cin >> students[i].dateOfBirth.day >> students[i].dateOfBirth.month >> students[i].dateOfBirth.year;
		std::cout << "Enter student #" << i + 1 << " sex (0 - female, 1 - male: ";
		std::cin >> students[i].sex;
	}
}

void PrintStudents(int const studentsCount, student* const students)
{
	for (size_t i = 0; i < studentsCount; i++)
	{
		std::cout
			<< std::setw(11) << students[i].firstName
			<< std::setw(11) << students[i].lastName
			<< std::setw(3) << students[i].dateOfBirth.day << "."
			<< std::setw(3) << students[i].dateOfBirth.month << "."
			<< std::setw(5) << students[i].dateOfBirth.year << " "
			<< (students[i].sex == 0 ? "Female" : students[i].sex == 1 ? "Male" : "Unknown")
			<< std::endl;
	}
}