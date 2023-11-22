import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import * as React from 'react';
import { StatusBar } from 'react-native';
import CartScreen from './screens/CartScreen';
import DeliveryScreen from './screens/DeliveryScreen';
import HomeScreen from './screens/HomeScreen';
import LoginScreen from './screens/LoginScreen';
import OrderListScreen from './screens/OrderListScreen';
import PreparingOrderScreen from './screens/PreparingOrderScreen';
import ProductListScreen from './screens/ProductListScreen';
import ResturantScreen from './screens/ResturantScreen';
import SignUpScreen from './screens/SignUpScreen';


const Stack = createNativeStackNavigator();

export default function Navigation() {
  const DEFAULT_WHITE = '#FFFFFF';

  return (
    <NavigationContainer>
         <StatusBar
                backgroundColor={DEFAULT_WHITE}
                options={{ presentation: 'fullScreenModal', headerShown: false}}
            />
        <Stack.Navigator initialRouteName='Home' options={{ presentation: 'fullScreenModal'}}>
            <Stack.Screen name="Login" options={{ headerShown: false}} component={LoginScreen} />
            <Stack.Screen name="SignUp" options={{headerShown: false}} component={SignUpScreen} />
            <Stack.Screen name="Home" options={{ presentation: 'fullScreenModal',headerShown: false}} component={HomeScreen} />
            <Stack.Screen name="ProductList" options={{ headerShown: false }} component={ProductListScreen} />
            <Stack.Screen name="Resturant" options={{ presentation: 'fullScreenModal',headerShown: false}} component={ResturantScreen} />
            <Stack.Screen name="Cart" options={{ presentation: 'modal', headerShown: false }} component={CartScreen} />
            <Stack.Screen name="OrderList" options={{ presentation: 'modal', headerShown: false }} component={OrderListScreen} />
            <Stack.Screen name="PreparingOrder" options={{ presentation: 'fullScreenModal', headerShown: false }} component={PreparingOrderScreen} />
            <Stack.Screen name="Delivery" options={{ presentation: 'fullScreenModal', headerShown: false }} component={DeliveryScreen} />
        </Stack.Navigator>
    </NavigationContainer>
  );
}