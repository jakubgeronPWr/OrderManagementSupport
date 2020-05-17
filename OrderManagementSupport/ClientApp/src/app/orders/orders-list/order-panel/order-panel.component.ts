import { Component, Input, OnInit } from '@angular/core';
import { Order } from '../../../shared/model/order';

@Component({
  selector: 'app-order-panel',
  templateUrl: './order-panel.component.html',
  styleUrls: ['./order-panel.component.css']
})
export class OrderPanelComponent implements OnInit {

  @Input()
  order: Order;

  constructor() { }

  ngOnInit(): void {
  }

}
