import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookingDetailPanelComponent } from './booking-detail-panel.component';

describe('BookingDetailPanelComponent', () => {
  let component: BookingDetailPanelComponent;
  let fixture: ComponentFixture<BookingDetailPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BookingDetailPanelComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookingDetailPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
