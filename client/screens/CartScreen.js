import { useNavigation } from '@react-navigation/native';
import React, { useMemo, useState } from 'react';
import { Image, ScrollView, Text, TouchableOpacity, View } from 'react-native';
import * as Icon from "react-native-feather";
import { useDispatch, useSelector } from 'react-redux';
import { urlFor } from '../sanity';
import { removeFromBasket, selectBasketItems, selectBasketTotal } from '../slices/basketSlice';
import { selectResturant } from '../slices/resturantSlice';
import { themeColors } from '../theme';

export default function BasketScreen() {
    const resturant = useSelector(selectResturant); 
    const [groupedItems, setGroupedItems] = useState([])
    const basketItems = useSelector(selectBasketItems);
    const basketTotal = useSelector(selectBasketTotal);

    const dispatch = useDispatch();
    const navigation = useNavigation();
    const deliveryFee = 2;
    useMemo(() => {
        const gItems = basketItems.reduce((group, item)=>{
            if(group[item.id]){
              group[item.id].push(item);
            }else{
              group[item.id] = [item];
            }
            return group;
          },{})
        setGroupedItems(gItems);
       
    }, [basketItems])
    
  return (
    <View className=" bg-white flex-1">
      {/* top button */}
      <View className="relative py-4 shadow-sm">
        <TouchableOpacity 
            style={{backgroundColor: themeColors.bgColor(1)}} 
            onPress={navigation.goBack} 
            className="absolute z-10 rounded-full p-1 shadow top-5 left-2">
            <Icon.ArrowLeft strokeWidth={3} stroke="white" />
        </TouchableOpacity>
        <View>
            <Text className="text-center font-bold text-xl">Giỏ hàng</Text>
            <Text className="text-center text-gray-800">{resturant.title}</Text>
        </View>
        
      </View>

     {/* delivery time */}
      <View style={{backgroundColor: themeColors.bgColor(0.2)}} className="flex-row px-4 items-center">
            <Image source={require('../assets/images/bikeGuy.png')} className="w-20 h-20 rounded-full" />
            <Text className="flex-1 pl-4">Giao hàng sau 20-30 phút</Text>
            <TouchableOpacity>
                <Text style={{color: themeColors.text}} className="font-bold">Thay đổi</Text>
            </TouchableOpacity>
      </View>

      {/* dishes */}
      <ScrollView 
      showsVerticalScrollIndicator={false}
       className="bg-white pt-5"
       contentContainerStyle={{
        paddingBottom: 50
       }}
       
       >
            {
                Object.entries(groupedItems).map(([key, items])=>{
                    return (
                        <View key={key} 
                        className="flex-row items-center space-x-3 py-2 px-4 bg-white rounded-3xl mx-2 mb-3 shadow-md">
                            <Text style={{color: themeColors.text}} className="font-bold">{items.length} x </Text>
                            <Image className="h-20 w-20 rounded-lg" source={{uri: urlFor(items[0]?.image).url()}} />
                            <Text className="flex-1 font-bold text-gray-700">{items[0]?.name}</Text>
                            <Text className="font-semibold text-base">{items[0]?.price * items.length}</Text>
                            <TouchableOpacity 
                                className="p-1 rounded-full" 
                                style={{backgroundColor: themeColors.bgColor(1)}} 
                                onPress={()=> dispatch(removeFromBasket({id: items[0]?.id}))}>
                                <Icon.Minus strokeWidth={2} height={20} width={20} stroke="white" />
                            </TouchableOpacity>
                        </View>
                    )
                })
            }
        </ScrollView>
     {/* totals */}
      <View style={{backgroundColor: themeColors.bgColor(0.2)}} className=" p-6 px-8 rounded-t-3xl space-y-4">
            <View className="flex-row justify-between">
                <Text className="text-gray-700">Tổng thu</Text>
                <Text className="text-gray-700">{basketTotal}</Text>
            </View>
            <View className="flex-row justify-between">
                <Text className="text-gray-700">Phí giao hàng</Text>
                <Text className="text-gray-700">{deliveryFee}</Text>
            </View>
            <View className="flex-row justify-between">
                <Text className="font-extrabold">Tổng số đơn hàng</Text>
                <Text className="font-extrabold">{basketTotal}</Text>
            </View>
            <View>
                <TouchableOpacity 
                style={{backgroundColor: themeColors.bgColor(1)}} 
                onPress={()=> navigation.navigate('PreparingOrder')} 
                className="p-3 rounded-full">
                    <Text className="text-white text-center font-bold text-lg">Đặt hàng</Text>
                </TouchableOpacity>
            </View>
       </View>
    </View>
  )
}
