import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Dashboard {
  customers: number;
  accounts: number;
  devices: number;
  ipAddresses: number;
  transfers: number;
}

export interface Customer {
  customerId: string;
  name: string;
  email: string;
}

export interface CustomerDetails extends Customer {
  accounts: string[];
  devices: string[];
  ipAddresses: string[];
  city: string;
}

export interface SharedDevice {
  customerName: string;
  deviceName: string;
  sharedWith: string;
}

export interface SharedIp {
  customerName: string;
  ipAddress: string;
  sharedWith: string;
}

export interface TransferChain {
  startAccount: string;
  path: string[];
}

export interface FraudScore {
  customerName: string;
  score: number;
  reasons: string[];
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private readonly baseUrl = 'https://localhost:7213/api';

  constructor(private http: HttpClient) {}

  getDashboard(): Observable<Dashboard> {
    return this.http.get<Dashboard>(
      `${this.baseUrl}/Dashboard`
    );
  }

  searchCustomers(name: string): Observable<Customer[]> {
    const params = new HttpParams().set('name', name);

    return this.http.get<Customer[]>(
      `${this.baseUrl}/Customer/search`,
      { params }
    );
  }

  getCustomer(id: string): Observable<CustomerDetails> {
    return this.http.get<CustomerDetails>(
      `${this.baseUrl}/Customer/${id}`
    );
  }

  getSharedDevices(name: string): Observable<SharedDevice[]> {
    return this.http.get<SharedDevice[]>(
      `${this.baseUrl}/Fraud/shared-device/${encodeURIComponent(name)}`
    );
  }

  getSharedIps(name: string): Observable<SharedIp[]> {
    return this.http.get<SharedIp[]>(
      `${this.baseUrl}/Fraud/shared-ip/${encodeURIComponent(name)}`
    );
  }

  getTransferChain(account: string): Observable<TransferChain> {
    return this.http.get<TransferChain>(
      `${this.baseUrl}/Fraud/transfer-chain/${account}`
    );
  }

  getFraudScore(name: string): Observable<FraudScore> {
    return this.http.get<FraudScore>(
      `${this.baseUrl}/Fraud/score/${encodeURIComponent(name)}`
    );
  }
}