import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CategoriesComponent, IncomeCategory } from '../categories/categories.component';
import { IncomeDto, IncomeClient } from '../web-api-client';

@Component({
  selector: 'app-incomes',
  templateUrl: './incomes.component.html',
  styleUrls: ['./incomes.component.css']
})
export class IncomesComponent implements OnInit {
  userId: string = '';
  selectedMonth = new Date().getMonth() + 1;
  selectedYear = new Date().getFullYear();
  incomes: IncomeDto[] = [];

  constructor(
    private incomesClient: IncomeClient
  ) {} 

  ngOnInit(): void {
    const user = sessionStorage.getItem(`oidc.user:https://localhost:44471:Finances`);
    const parsedUser = JSON.parse(user || '');
    this.userId = parsedUser.profile.sub;
    const currentDate = new Date();

    this.incomesClient.income_GetAll(this.selectedMonth, this.selectedYear, this.userId).subscribe(result => {
      this.incomes = result.list!;
    }, error => console.error(error));
  }

  filterIncomes(month: number, year: number) {
    this.incomesClient.income_GetAll(month, year, this.userId).subscribe(result => {
      this.incomes = result.list!;
    }, error => console.error(error));
  }
}


