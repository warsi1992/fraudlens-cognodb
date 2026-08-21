import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import {
  ApiService,
  CustomerDetails,
  SharedDevice,
  SharedIp,
  TransferChain,
  FraudScore
} from '../../services/api.service';

@Component({
  selector: 'app-customer-investigation',
  standalone: true,
  imports: [],
  templateUrl: './customer-investigation.component.html',
  styleUrl: './customer-investigation.component.scss'
})
export class CustomerInvestigationComponent implements OnInit {

  customer?: CustomerDetails;

  sharedDevices: SharedDevice[] = [];

  sharedIps: SharedIp[] = [];

  transferChain?: TransferChain;

  fraudScore?: FraudScore;

  loading = true;

  error = '';

  constructor(
    private api: ApiService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {

    const customerId = this.route.snapshot.paramMap.get('customerId');

    if (!customerId) {
      this.error = 'Customer ID was not provided.';
      this.loading = false;
      return;
    }

    this.loadCustomer(customerId);
  }

  loadCustomer(customerId: string): void {

    this.loading = true;

    this.api.getCustomer(customerId).subscribe({
      next: result => {

        this.customer = result;

        this.loading = false;

        this.loadFraudData(result.name);

        if (result.accounts?.length > 0) {
          this.loadTransferChain(result.accounts[0]);
        }

      },
      error: error => {

        console.error('Customer error:', error);

        this.error = 'Unable to load customer information.';

        this.loading = false;
      }
    });
  }

  loadFraudData(name: string): void {

    this.api.getSharedDevices(name).subscribe({
      next: result => {
        this.sharedDevices = result;
      },
      error: error => {
        console.error('Shared devices error:', error);
      }
    });

    this.api.getSharedIps(name).subscribe({
      next: result => {
        this.sharedIps = result;
      },
      error: error => {
        console.error('Shared IP error:', error);
      }
    });

    this.api.getFraudScore(name).subscribe({
      next: result => {
        this.fraudScore = result;
      },
      error: error => {
        console.error('Fraud score error:', error);
      }
    });
  }

  loadTransferChain(account: string): void {

    this.api.getTransferChain(account).subscribe({
      next: result => {
        this.transferChain = result;
      },
      error: error => {
        console.error('Transfer chain error:', error);
      }
    });
  }

  backToSearch(): void {
    this.router.navigate(['/search']);
  }
}