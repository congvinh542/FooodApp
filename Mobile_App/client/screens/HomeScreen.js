import { useNavigation, useRoute } from '@react-navigation/native'
import React, { useEffect, useLayoutEffect, useState } from 'react'
import { SafeAreaView, ScrollView, StatusBar, Text, TextInput, TouchableOpacity, View } from 'react-native'
import * as Icon from "react-native-feather"
import { getFeaturedResturants } from '../api'
import Categories from '../components/categories'
import FeatureRow from '../components/featuredRow'

export default function HomeScreen() {

    const [featuredCategories, setFeaturedCategories] = useState([])
    const navigation = useNavigation();
    const route = useRoute();
    const { isLoggedIn, userId } = route.params || {};


    useEffect(() => {
        if (isLoggedIn) {
            // Xử lý khi đăng nhập thành công, có thể làm gì đó với userData
           // console.log('User data:', userData);
        }
    }, [isLoggedIn, userId]);

    useLayoutEffect(() => {
      navigation.setOptions({headerShown: false})
    }, [])
    useEffect(()=>{
        getFeaturedResturants().then(data=>{
            setFeaturedCategories(data);
        })
    },[])

    const handleIconPress = () => {
        if (isLoggedIn) {
            navigation.navigate('OrderList', { userId: userId });
        } else {
            navigation.navigate('Login');
        }
    };
  return (
    <SafeAreaView className="bg-white pt-3 flex-1">
    <StatusBar
        barStyle="dark-content" 
    />
    {/* search bar */}
        <View className="flex-row items-center space-x-2 px-4 pb-2 ">
            <View className="flex-row flex-1 items-center p-3 rounded-full border border-gray-300">
                <Icon.Search height="25" width="25" stroke="gray" />
                <TextInput placeholder='Resturants' className="ml-2 flex-1" keyboardType='default' />
                <View className="flex-row items-center space-x-1 border-0 border-l-2 pl-2 border-l-gray-300">
                    <Icon.MapPin height="20" width="20" stroke="gray" />
                    <Text className="text-gray-600">Nha Trang</Text>
                </View>
            </View>
              <TouchableOpacity onPress={handleIconPress}>
                  <View style={{ backgroundColor: 'gray', padding: 12, borderRadius: 30 }}>
                      {isLoggedIn ? (
                          <Icon.User height={23} width={23} strokeWidth={2.5} stroke="white" />
                      ) : (
                          <Icon.Menu height={23} width={23} strokeWidth={2.5} stroke="white" />
                      )}
                  </View>
              </TouchableOpacity>
        </View>

    {/* main */}
    <ScrollView
        showsVerticalScrollIndicator={false}
        contentContainerStyle={{
            paddingBottom: 50
        }}
    >
       
        {/* categories */}
        <Categories />

        {/* featured */}
        <View className="mt-5">
        {
            featuredCategories?.map(category=>{
                return (
                        <FeatureRow 
                            key={category._id}
                            id={category._id}
                            title={category.name}
                            resturants={category?.resturants}
                            description={category.description}
                            featuredCategory={category._type}
                        />
                )
            })
        }
        </View>
        

        
       
    </ScrollView>
      
    </SafeAreaView>
  )
}
