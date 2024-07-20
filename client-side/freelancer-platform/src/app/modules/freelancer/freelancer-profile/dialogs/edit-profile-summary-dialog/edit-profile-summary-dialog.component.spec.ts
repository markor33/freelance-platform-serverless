import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditProfileSummaryDialogComponent } from './edit-profile-summary-dialog.component';

describe('EditProfileSummaryDialogComponent', () => {
  let component: EditProfileSummaryDialogComponent;
  let fixture: ComponentFixture<EditProfileSummaryDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditProfileSummaryDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditProfileSummaryDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
