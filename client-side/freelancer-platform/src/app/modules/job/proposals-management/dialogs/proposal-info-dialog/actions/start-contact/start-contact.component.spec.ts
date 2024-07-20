import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StartContactComponent } from './start-contact.component';

describe('StartContactComponent', () => {
  let component: StartContactComponent;
  let fixture: ComponentFixture<StartContactComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StartContactComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StartContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
