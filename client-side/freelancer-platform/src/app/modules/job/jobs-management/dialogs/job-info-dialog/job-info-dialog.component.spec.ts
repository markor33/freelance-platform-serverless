import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobInfoDialogComponent } from './job-info-dialog.component';

describe('JobInfoDialogComponent', () => {
  let component: JobInfoDialogComponent;
  let fixture: ComponentFixture<JobInfoDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JobInfoDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(JobInfoDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
