import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditEmploymentDialogComponent } from './edit-employment-dialog.component';

describe('EditEmploymentDialogComponent', () => {
  let component: EditEmploymentDialogComponent;
  let fixture: ComponentFixture<EditEmploymentDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditEmploymentDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditEmploymentDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
