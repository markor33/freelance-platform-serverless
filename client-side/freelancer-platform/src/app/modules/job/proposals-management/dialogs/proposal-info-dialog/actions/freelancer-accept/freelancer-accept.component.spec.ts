import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FreelancerAcceptComponent } from './freelancer-accept.component';

describe('FreelancerAcceptComponent', () => {
  let component: FreelancerAcceptComponent;
  let fixture: ComponentFixture<FreelancerAcceptComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FreelancerAcceptComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FreelancerAcceptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
