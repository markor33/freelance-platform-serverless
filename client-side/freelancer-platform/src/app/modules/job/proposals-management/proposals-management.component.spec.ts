import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProposalsManagementComponent } from './proposals-management.component';

describe('ProposalsManagementComponent', () => {
  let component: ProposalsManagementComponent;
  let fixture: ComponentFixture<ProposalsManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProposalsManagementComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProposalsManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
