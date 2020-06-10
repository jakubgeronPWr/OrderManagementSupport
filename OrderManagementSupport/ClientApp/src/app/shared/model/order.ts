export interface Order {
  id: number;
  orderNumber: string;
  orderDate: string;
  orderRealizationDate: string;
  service: string;
  price: number;
  isPayed: boolean;
  isDone: boolean;
  clientId: number;
}
