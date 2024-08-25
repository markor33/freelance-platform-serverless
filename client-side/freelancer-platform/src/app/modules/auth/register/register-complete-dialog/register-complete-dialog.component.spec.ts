import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterCompleteDialogComponent } from './register-complete-dialog.component';

describe('RegisterCompleteDialogComponent', () => {
  let component: RegisterCompleteDialogComponent;
  let fixture: ComponentFixture<RegisterCompleteDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegisterCompleteDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegisterCompleteDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
