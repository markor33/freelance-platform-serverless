import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCertificationDialogComponent } from './add-certification-dialog.component';

describe('AddCertificationDialogComponent', () => {
  let component: AddCertificationDialogComponent;
  let fixture: ComponentFixture<AddCertificationDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddCertificationDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddCertificationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
