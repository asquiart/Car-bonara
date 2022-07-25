import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CarbonaraIconComponent } from './carbonara-icon.component';

describe('CarbonaraIconComponent', () => {
  let component: CarbonaraIconComponent;
  let fixture: ComponentFixture<CarbonaraIconComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CarbonaraIconComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CarbonaraIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
