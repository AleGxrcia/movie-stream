import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './styles.css';
import { RouterProvider } from 'react-router-dom';
import router from './routes';
import { Provider } from 'react-redux';
import { store } from './app/store';
import apiClient from './services/apiClient';
import { setupInterceptors } from './services/axiosInterceptors';
import apiClientAuth from './services/apiAuth';
import { checkAuth } from './features/auth/slices/authSlice';

setupInterceptors(apiClient, store);
setupInterceptors(apiClientAuth, store);

store.dispatch(checkAuth());

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider store={store}>
      <RouterProvider router={router} />
    </Provider>
  </StrictMode>,
)
