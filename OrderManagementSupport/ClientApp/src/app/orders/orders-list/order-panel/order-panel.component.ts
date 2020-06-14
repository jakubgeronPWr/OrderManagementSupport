import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Order } from '../../../shared/model/order';

@Component({
  selector: 'app-order-panel',
  templateUrl: './order-panel.component.html',
  styleUrls: ['./order-panel.component.css']
})
export class OrderPanelComponent implements OnInit {

  @Input()
  order: Order;

  @Output() 
  delete: EventEmitter<any> = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
  }

  deleteOrder(){
    this.delete.emit(this.order);
  }

}
