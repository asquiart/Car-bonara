import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CarclassFormComponent } from './carclass-form.component';

describe('CarclassFormComponent', () => {
  let component: CarclassFormComponent;
  let fixture: ComponentFixture<CarclassFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CarclassFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CarclassFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
