import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEmploymentDialogComponent } from './add-employment-dialog.component';

describe('AddEmploymentDialogComponent', () => {
  let component: AddEmploymentDialogComponent;
  let fixture: ComponentFixture<AddEmploymentDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddEmploymentDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddEmploymentDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
