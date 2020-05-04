export interface Order {
  id: number;
  orderNumber: string;
  orderDate: string;
  orderRealizationDate: string;
  service: string;
  price: number;
  isPayed: boolean;
  clientId: number;
}
