export enum UserRole {
  RestaurantManager,
}

export class User {
  readonly id: string;
  readonly name: string;
  readonly email: string;
  readonly role: UserRole;

  constructor(id: string, name: string, email: string, role: UserRole) {
    this.id = id;
    this.name = name;
    this.email = email;
    this.role = role;
  }
}
