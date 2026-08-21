import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import {
  ApiService,
  Customer
} from '../../services/api.service';

@Component({
  selector: 'app-customer-search',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './customer-search.component.html',
  styleUrl: './customer-search.component.scss'
})
export class CustomerSearchComponent {

  searchText = '';

  customers: Customer[] = [];

  loading = false;

  searched = false;

  error = '';

  constructor(
    private api: ApiService,
    private router: Router
  ) {}

  search(): void {

    const value = this.searchText.trim();

    if (!value) {
      this.customers = [];
      this.searched = false;
      return;
    }

    this.loading = true;
    this.searched = true;
    this.error = '';

    this.api.searchCustomers(value).subscribe({
      next: result => {
        this.customers = result;
        this.loading = false;
      },
      error: error => {
        console.error('Search error:', error);
        this.error = 'Unable to search customers.';
        this.customers = [];
        this.loading = false;
      }
    });
  }

  selectCustomer(customer: Customer): void {
    this.router.navigate(['/customer', customer.customerId]);
  }
}