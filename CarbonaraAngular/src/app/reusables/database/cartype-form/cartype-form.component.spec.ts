import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CartypeFormComponent } from './cartype-form.component';

describe('CartypeFormComponent', () => {
  let component: CartypeFormComponent;
  let fixture: ComponentFixture<CartypeFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CartypeFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CartypeFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
