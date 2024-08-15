import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompleteRegisterDialogComponent } from './profile-setup-dialog.component';

describe('CompleteRegisterDialogComponent', () => {
  let component: CompleteRegisterDialogComponent;
  let fixture: ComponentFixture<CompleteRegisterDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompleteRegisterDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CompleteRegisterDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
