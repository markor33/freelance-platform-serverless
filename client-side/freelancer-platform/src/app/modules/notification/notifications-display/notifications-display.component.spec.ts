import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationsDisplayComponent } from './notifications-display.component';

describe('NotificationsDisplayComponent', () => {
  let component: NotificationsDisplayComponent;
  let fixture: ComponentFixture<NotificationsDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NotificationsDisplayComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NotificationsDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
