import { useNavigation } from '@react-navigation/native';
import React from 'react';
import { Image, Text, TouchableWithoutFeedback, View } from 'react-native';
import * as Icon from "react-native-feather";
import { urlFor } from '../sanity';

export default function ResturantCard({
    id, 
    title,
    imgUrl,
    rating,
    type,
    address, 
    description,
    dishes,
    reviews,
    lng,
    lat
}) {
  const navigation = useNavigation();
  return (
    <TouchableWithoutFeedback style={{width: 250,height: 250}} onPress={()=>{
      navigation.navigate('Resturant', {
        id, 
        title,
        imgUrl,
        rating,
        type,
        address, 
        description,
        dishes,
        lng,
        reviews,
        lat
      })
    }}>
      <View 
      style={{
        shadowRadius: 7,
        shadowColor: 'rgba(0, 0, 0, 0.9)',
        shadowOffset: {
          width: 0,
          height: 4,
        },
        shadowOpacity: 1,
        elevation: 3,
        borderRadius: 10,
        marginBottom: 10
      }}
      className='mr-6 bg-white rounded-3lg shadow-lg'>
          <Image  className="rounded-t-xl" style={{width: 300, height:250}} source={{ uri: urlFor(imgUrl).url()}} />
        
        <View className="px-3 pb-4 space-y-2">
         
          <Text className="text-lg font-bold pt-2">{title}</Text>
          <View className="flex-row items-center space-x-1">
              <Image source={require('../assets/images/fullStar.png')} className="h-4 w-4" />
              <Text className="text-xs">
                  <Text className="text-green-700">{rating}</Text>
                  <Text className="text-gray-700"> ({reviews} review)</Text> · <Text className="font-semibold text-gray-700">{type}</Text>
              </Text>
          </View>
          <View className="flex-row items-center space-x-1">
              <Icon.MapPin color="gray" width={15} height={15} />
              <Text className="text-gray-700 text-xs">{address}</Text>
          </View>
        </View>
      </View>
    </TouchableWithoutFeedback>
    
  )
}

