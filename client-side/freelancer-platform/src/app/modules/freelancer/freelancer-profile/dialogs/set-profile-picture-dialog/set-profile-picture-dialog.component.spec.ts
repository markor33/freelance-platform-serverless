import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetProfilePictureDialogComponent } from './set-profile-picture-dialog.component';

describe('SetProfilePictureDialogComponent', () => {
  let component: SetProfilePictureDialogComponent;
  let fixture: ComponentFixture<SetProfilePictureDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SetProfilePictureDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SetProfilePictureDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
