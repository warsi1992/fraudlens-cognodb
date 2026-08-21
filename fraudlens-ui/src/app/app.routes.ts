import { Routes } from '@angular/router';

import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { CustomerSearchComponent } from './pages/customer-search/customer-search.component';
import { CustomerInvestigationComponent } from './pages/customer-investigation/customer-investigation.component';

export const routes: Routes = [

  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },

  {
    path: 'dashboard',
    component: DashboardComponent
  },

  {
    path: 'search',
    component: CustomerSearchComponent
  },

  {
    path: 'customer/:customerId',
    component: CustomerInvestigationComponent
  },

  {
    path: '**',
    redirectTo: 'dashboard'
  }

];