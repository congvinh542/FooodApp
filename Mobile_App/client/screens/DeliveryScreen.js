import { useNavigation } from '@react-navigation/native';
import React from 'react';
import { Image, Linking, Text, TouchableOpacity, View } from 'react-native';
import * as Icon from "react-native-feather";
import MapView, { Marker } from 'react-native-maps';
import { useDispatch, useSelector } from 'react-redux';
import { emptyBasket } from '../slices/basketSlice';
import { selectResturant } from '../slices/resturantSlice';
import { themeColors } from '../theme';


export default function DeliveryScreen() {
    const navigation = useNavigation();
    const resturant = useSelector(selectResturant);
    const dispatch = useDispatch();
    const phoneNumber = '0329471013';
    const handlePhonePress = () => {
    Linking.openURL(`tel:${phoneNumber}`);
};
    const handleCancel = ()=>{
      dispatch(emptyBasket());
      navigation.navigate('Home')
    }
  return (
    <View className="flex-1" >
        <MapView
          initialRegion={{
            latitude: 13.24346,
            longitude: 109.22369,
            latitudeDelta: 0.01,
            longitudeDelta: 0.01,
          }} 
          style={{ flex: 1 }}
          mapType="standard"
            >
              <Marker 
                coordinate={{
                  latitude: 13.24346,
                  longitude: 109.22369
                }} 
                title={resturant.title}
                description={resturant.description}
                pinColor={themeColors.bgColor(1)}
            />
        </MapView>
        
      <View className="rounded-t-3xl -mt-12 bg-white relative">
          <TouchableOpacity className="absolute right-4 top-2">
            
          </TouchableOpacity>
          <View className="flex-row justify-between px-5 pt-10">
              <View>
                  <Text className="text-lg text-gray-700 font-semibold">Thời gian dự kiến</Text>
                  <Text className="text-3xl font-extrabold text-gray-700">20-30 phút</Text>
                  <Text className="mt-2 text-gray-700 font-semibold">Đơn hàng của bạn đang được vận chuyển</Text>
              </View>
              <Image className="h-24 w-24" source={require('../assets/images/bikeGuy2.gif')} />
          </View>
          
        <View 
          style={{backgroundColor: themeColors.bgColor(0.8)}} 
          className="p-2 flex-row justify-between items-center rounded-full my-5 mx-2">
            <View style={{backgroundColor: 'rgba(255,255,255,0.4)'}} className="p-1 rounded-full">
              <Image style={{backgroundColor: 'rgba(255,255,255,0.4)'}} className="w-16 h-16 rounded-full" source={require('../assets/images/deliveryGuy.png')} />
            </View>
            
            <View className="flex-1 ml-3">
                <Text className="text-lg font-bold text-white">Công Vinh</Text>
                <Text className="text-white font-semibold">Shipper</Text>
            </View>
            <View className="flex-row items-center space-x-3 mr-3">
              <TouchableOpacity className="bg-white p-2 rounded-full" onPress={handlePhonePress}>
                <Icon.Phone fill={themeColors.bgColor(1)} href={'0963562615'} stroke={themeColors.bgColor(1)} strokeWidth="1" />
              </TouchableOpacity>
              
              <TouchableOpacity onPress={handleCancel} className="bg-white p-2 rounded-full">
                <Icon.X stroke={'red'} strokeWidth="5" />
              </TouchableOpacity>
              
            </View>
            
        </View>
      </View>
    </View>
  )
}