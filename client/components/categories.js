import React, { useEffect, useState } from 'react';
import { Image, ScrollView, Text, TouchableOpacity, View } from 'react-native';
import { getCategories } from '../api';
import { urlFor } from '../sanity';

export default function Categories() {
 
  const [activeCategory, setActiveCategory] = useState(null);
  const [categories, setCategories] = useState([])
  useEffect(() => {
    getCategories().then(data=>{
      setCategories(data);
    })
  }, [])
  
  return (
   <View className='mt-4'>
      <ScrollView 
      horizontal
      showsHorizontalScrollIndicator={false}
      className='overflow-visible'
      contentContainerStyle={{
        paddingHorizontal: 15
      }}>
        {
            categories.map((category, index)=>{
                let isActive = category._id==activeCategory
                let btnClass = isActive? 'bg-gray-600' : 'bg-gray-200'
                let textClass = isActive? 'font-semibold text-orange-500' : 'text-gray-800'
                return (
                    <View key={index} 
                    className='flex justify-center items-center mr-6'>
                        <TouchableOpacity 
                        onPress={() => setActiveCategory(category._id)}
                        className={'rounded-full shadow bg-gray-200 ' + btnClass}>
                            <Image style={{width: 80, height: 80, borderRadius: 50}} source={{uri: urlFor(category.image).url()}}/>
                        </TouchableOpacity>
                            <Text className={'text-base '+ textClass} >{category.name}</Text>
                    </View>
                )
            })
        }
      </ScrollView>
    </View>
  )
}
