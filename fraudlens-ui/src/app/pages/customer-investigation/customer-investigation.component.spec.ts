import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerInvestigationComponent } from './customer-investigation.component';

describe('CustomerInvestigationComponent', () => {
  let component: CustomerInvestigationComponent;
  let fixture: ComponentFixture<CustomerInvestigationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerInvestigationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomerInvestigationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
