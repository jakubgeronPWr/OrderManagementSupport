import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderPanelComponent } from './order-panel.component';
import { Order } from '../../../shared/model/order';

describe('OrderPanelComponent', () => {
  let component: OrderPanelComponent;
  let fixture: ComponentFixture<OrderPanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrderPanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    component.order = {
      id: 0,
      orderNumber: "1",
      orderDate: "2020-06-11",
      orderRealizationDate: "2020-06-11",
      service: "6 or more message",
      price: 11,
      isPayed: false,
      isDone: true,
      clientId: 1
    };
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  it("should call a function", function(){
    expect(component.deleteOrder).toHaveBeenCalled();
  });
});
