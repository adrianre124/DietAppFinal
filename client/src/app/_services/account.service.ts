import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { User } from '../_models/user';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
 private http = inject(HttpClient);
 baseUrl = environment.apiUrl;
 currentUser = signal<User | null>(null);
 roles = computed(() => {
  const user = this.currentUser();
  if (user && user.token) {
    const role = JSON.parse(atob(user.token.split('.')[1])).role;
    return Array.isArray(role) ? role : [role];
  }
  return [];
 })

 login(model: any) {
  return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
    map(user => {
      if (user) {
        this.setCurrentUser(user);
      }
      return user;
    })
  )
 }

 logout() {
  localStorage.removeItem('user');
  this.currentUser.set(null);
 }

 register(model: any) {
  return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
    map(user => {
      if (user) {
        this.setCurrentUser(user);
      }
      return user;
    })
  )
 }

 setCurrentUser(user: User) {
  localStorage.setItem('user', JSON.stringify(user));
  this.currentUser.set(user);
 }
}
