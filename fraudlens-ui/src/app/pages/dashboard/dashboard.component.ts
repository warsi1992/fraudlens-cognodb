import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import {
  ApiService,
  Dashboard
} from '../../services/api.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {

  dashboard?: Dashboard;

  loading = true;

  error = '';

  constructor(
    private api: ApiService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {

    this.api.getDashboard().subscribe({
      next: result => {
        this.dashboard = result;
        this.loading = false;
      },
      error: error => {
        console.error('Dashboard error:', error);
        this.error = 'Unable to load dashboard.';
        this.loading = false;
      }
    });
  }

  openSearch(): void {
    this.router.navigate(['/search']);
  }
}